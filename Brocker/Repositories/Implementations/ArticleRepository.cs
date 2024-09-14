using Brocker.DbContexts;
using Brocker.Models;

namespace Brocker.Repositories.Implementations;

public class ArticleRepository: IArticleRepository
{
    private BrockerDbContext _dbContext = BrockerDbContext.GetBrockerDbContext();
    
    public Article CreateArticle(Article article)
    {
        _dbContext.Articles.Add(article);
        _dbContext.SaveChanges();
        
        CreateSendings(article);
        
        return article;
    }

    private void CreateSendings(Article article)
    {
        var subscriptions = _dbContext.Subscriptions.Where(sub => sub.TopicId == article.TopicId);

        foreach (var sub in subscriptions)
        {
            var sending = new Sending() { UserId = sub.UserId, ArticleId = article.Id };
            _dbContext.Sendings.Add(sending);
        }

        _dbContext.SaveChanges();
    }
}