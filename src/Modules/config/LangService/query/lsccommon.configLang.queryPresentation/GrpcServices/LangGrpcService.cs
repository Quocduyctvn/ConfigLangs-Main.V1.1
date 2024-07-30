using AutoMapper;
using Grpc.Core;
using LangGrpc;
using lsccommon.configLang.queryApplication.UserCases;
using lsccommon.configLang.queryContract.Enumerations;
using MediatR;

namespace lsccommon.configLang.queryPresentation.GrpcServices
{
	/// <summary>
	/// gRPC service for handling language-related requests
	/// </summary>
	public class LangGrpcService : Lang.LangBase
	{
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;

		// Constructor to initialize the service with dependency injection
		public LangGrpcService(IMapper mapper, IMediator mediator)
		{
			_mapper = mapper;
			_mediator = mediator;
		}


		/// <summary>
		/// gRPC method to get language information by ID
		/// </summary>
		/// <param name="request">Rpc get lang request</param>
		/// <param name="context">Context of request</param>
		/// <returns>Response contain lang</returns>
		/// <exception cref="RpcException">Exception will throw when resources not found or catch any exception</exception>
		public override async Task<GetLangResponse> GetLang(GetLangRequest request, ServerCallContext context)
		{
			var id = request.Id;
			var query = new GetLangByIdQuery(id);
			var result = await _mediator.Send(query);

			if (!result.IsSuccess)
			{
				switch (result.Error!.Type)
				{
					case ErrorType.NotFound:
						throw new RpcException(new Status(StatusCode.NotFound,
							$"Language with id '{id}' not found."));
					case ErrorType.ServerError:
						throw new RpcException(new Status(StatusCode.Internal, "Internal server error."));
				}
			}
			var data = _mapper.Map<GetLangResponse>(result.Value);
			return data;
		}


		/// <summary>
		/// Get all Langs
		/// </summary>
		/// <param name="request">Rpc get all Langs request</param>
		/// <param name="context">Context of request</param>
		/// <returns>Response contain list of langs</returns>
		/// <exception cref="RpcException">Exception will throw when catch any exception</exception>
		public override async Task<GetAllLangResponse> GetAllLang(GetAllLangRequest request, ServerCallContext context)
		{
			var query = new GetAllLangQuery();
			var result = await _mediator.Send(query);

			if (!result.IsSuccess)
			{
				throw new RpcException(new Status(StatusCode.Internal, "Internal server error."));
			}

			var data = _mapper.Map<List<GetLangResponse>>(result.Value);
			var response = new GetAllLangResponse
			{
				Lang = { data }
			};

			return response;
		}
	}
}