using UnityEngine;
using Zenject;
using AlphaECS.SurvivalShooter;

public class GroupsInstaller : MonoInstaller<GroupsInstaller>
{
    public override void InstallBindings()
    {
		Container.Bind<Deads>().To<Deads>().AsSingle();
    }
}