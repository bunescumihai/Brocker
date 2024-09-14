namespace Brocker.Repositories;

public interface IArticleRepository
{
    Article CreateArticle(Article article);

    List<Article> GetUnsentArticles(User user);

    void CancelSending(User user, IEnumerable<Article> articles);
}