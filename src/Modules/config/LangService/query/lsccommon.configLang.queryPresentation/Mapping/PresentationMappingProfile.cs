using AutoMapper;
using LangGrpc;
using Entities = lsccommon.configLang.queryDomain.Entities;

namespace lsccommon.configLang.queryPresentation.Mapping
{
	/// <summary>
	/// Defines mapping configurations for the presentation layer.
	/// </summary>
	public class PresentationMappingProfile : Profile
	{
		public PresentationMappingProfile()
		{
			CreateMap<Entities.Lang, GetLangResponse>();
		}
	}
}