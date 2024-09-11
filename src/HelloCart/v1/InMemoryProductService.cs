namespace Samples.HelloCart.V1;

public class InMemoryProductService : IProductService
{
    private readonly ConcurrentDictionary<string, Product> _products = new();

    public virtual Task Edit(EditCommand<Product> command, CancellationToken cancellationToken = default)
    {
        // Извлекает идентификатор продукта productId и сам продукт product из команды EditCommand<Product>
        var (productId, product) = command;

        if (string.IsNullOrEmpty(productId))
            throw new ArgumentOutOfRangeException(nameof(command));

        // Проверяет, является ли текущее вычисление инвалидацией (возможно в контексте кэширование или вычислений Stl.Fusion)
        // Если является, то вызывается Get, чтобы обновить данные 
        if (Computed.IsInvalidating()) {
            _ = Get(productId, default);
            // Завершение метода
            return Task.CompletedTask;
        }

        // Если product равен null, то продукт должен быть удалён, вызывается Remove по productId
        if (product == null)
            _products.Remove(productId, out _);
        // Иначе продукт должен быть добавлен или обновлён
        else
            _products[productId] = product;
        // Завершение метода
        return Task.CompletedTask;
    }

    public virtual Task<Product?> Get(string id, CancellationToken cancellationToken = default)
        => Task.FromResult(_products.GetValueOrDefault(id));

    public virtual Task Add(Product product, CancellationToken cancellationToken = default)
    {
        if(product is null) 
            throw new ArgumentNullException(nameof(product));
        if(string.IsNullOrEmpty(product.Id))
            throw new ArgumentException("Product must have id", nameof(product));
        if (Computed.IsInvalidating()) 
            {
            _ = Get(product.Id, default);
            return Task.CompletedTask;
            }

        _products[product.Id] = product;
        return Task.CompletedTask;

    }
}
