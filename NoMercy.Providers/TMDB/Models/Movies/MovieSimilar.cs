using NoMercy.Providers.TMDB.Models.Shared;

namespace NoMercy.Providers.TMDB.Models.Movies;

public class MovieSimilar : PaginatedResponse<SimilarMovie>
{
}
public class SimilarMovie : Movie
{
}