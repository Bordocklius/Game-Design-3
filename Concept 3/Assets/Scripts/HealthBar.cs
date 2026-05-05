using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HealthBar: IDisposable
    {
        private IHealthBar _model;
        public IHealthBar Model
        {
            get { return _model; }
            set
            {
                if (_model == value)
                    return;

                if (_model != null)
                {
                    _model.PropertyChanged -= Model_OnPropertyChanged;
                }

                _model = value;
                _model.PropertyChanged += Model_OnPropertyChanged;
            }
        }

        private Image _image;

        public HealthBar(IHealthBar healthbar, Image image)
        {
            Model = healthbar;
            _image = image;
        }

        public void Dispose()
        {
            if (Model != null)
            {
                Model.PropertyChanged -= Model_OnPropertyChanged;
            }
        }

        private void Model_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(IHealthBar.Health))
            {
                UpdateHealthbar();
            }
        }

        private void UpdateHealthbar()
        {
            _image.fillAmount = _model.HealthProgress;
        }
    }
}
