namespace Samples.HelloCart.V1;

public class AppV1 : AppBase
{
    public AppV1()
    {
        // Добавление сервисов <IProductService, InMemoryProductService>,
                             //<ICartService, InMemoryCartService>
        var services = new ServiceCollection();
        services.AddFusion(fusion => {
            fusion.AddService<IProductService, InMemoryProductService>();
            fusion.AddService<ICartService, InMemoryCartService>();
        });
        ClientServices = ServerServices = services.BuildServiceProvider();
    }
}
