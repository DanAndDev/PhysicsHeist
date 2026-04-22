using PhysicsHeist.Core.Signals;
using PhysicsHeist.Infrastructure.Bootstrap;
using Zenject;

namespace PhysicsHeist.Infrastructure.DI
{
    public sealed class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<BootCompletedSignal>();

            Container.Bind<ISignalBus>().To<ZenjectSignalBus>().AsSingle();

            Container.BindInterfacesTo<BootstrapFlow>().AsSingle().NonLazy();
        }
    }
}
