using Brocker.DbContexts;

namespace Brocker.Repositories.Implementations;

public class ArticleRepository: IArticleRepository
{
    private BrockerDbContext _dbContext = BrockerDbContext.GetBrockerDbContext();
    
    public Article CreateArticle(Article article)
    {
        _dbContext.Articles.Add(article);

        _dbContext.SaveChanges();
        
        return article;
    }
}