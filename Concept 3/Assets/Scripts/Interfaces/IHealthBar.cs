using System.ComponentModel;

namespace Assets.Scripts.Interfaces
{
    public interface IHealthBar: INotifyPropertyChanged
    {
        public float Health { get; }
        public float HealthProgress { get; }

    }
}
