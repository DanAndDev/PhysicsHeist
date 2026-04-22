using PhysicsHeist.Core.Input;
using PhysicsHeist.Core.Signals;
using PhysicsHeist.Infrastructure.Bootstrap;
using PhysicsHeist.Infrastructure.Input;
using Zenject;

namespace PhysicsHeist.Infrastructure.DI
{
    public sealed class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<BootCompletedSignal>();
            Container.DeclareSignal<ObjectBrokenSignal>();

            Container.Bind<ISignalBus>().To<ZenjectSignalBus>().AsSingle();

            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();

            Container.BindInterfacesTo<BootstrapFlow>().AsSingle().NonLazy();
        }
    }
}
