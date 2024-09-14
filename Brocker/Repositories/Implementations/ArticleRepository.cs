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

    public List<Article> GetUnsentArticles(User user)
    {
        return _dbContext.Sendings
            .Where(ua => ua.UserId == user.Id)
            .Select(ua => ua.Article)
            .ToList();
    }

    public void CancelSending(User user, IEnumerable<Article> articles)
    {
        var articleIds = articles.Select(a => a.Id).ToList();

        
        var userArticlesToRemove = _dbContext.Sendings
            .Where(ua => ua.UserId == user.Id && articleIds.Contains(ua.ArticleId))
            .ToList();
        
        _dbContext.Sendings.RemoveRange(userArticlesToRemove);
        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine("???");
        }
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