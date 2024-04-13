using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieRecommendations : PaginatedResponse<RecommendationsMovie>
{
}
public class RecommendationsMovie : Movie
{
}