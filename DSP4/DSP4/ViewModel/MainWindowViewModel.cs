using DSP4.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DSP4.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private SmoothingType _smoothingType;
        private int _signalsCount;
        private int _harmonicsCount;
        private IEnumerable<Vector> _signals;
        private IEnumerable<Vector> _amplitudeSpectrums;
        private IEnumerable<Vector> _phaseSpectrums;
        private IEnumerable<Vector> _smoothedSignals;

        public MainWindowViewModel()
        {
            _smoothingType = SmoothingType.Parabolic;
            _signalsCount = 512;
            _harmonicsCount = 8;
            PropertyChanged += OnPropertyChanged;
            Update();
        }

        public int SignalsCount { get => _signalsCount; set => SetProperty(ref _signalsCount, value, nameof(SignalsCount)); }

        public int HarmonicsCount { get => _harmonicsCount; set => SetProperty(ref _harmonicsCount, value, nameof(HarmonicsCount)); }

        public SmoothingType SmoothingType { get => _smoothingType; set => SetProperty(ref _smoothingType, value, nameof(SmoothingType)); }

        public IEnumerable<Vector> Signals { get => _signals; set => SetProperty(ref _signals, value, nameof(Signals)); }

        public IEnumerable<Vector> SmoothedSignals { get => _smoothedSignals; set => SetProperty(ref _smoothedSignals, value, nameof(SmoothedSignals)); }

        public IEnumerable<Vector> AmplitudeSpectrums { get => _amplitudeSpectrums; set => SetProperty(ref _amplitudeSpectrums, value, nameof(AmplitudeSpectrums)); }

        public IEnumerable<Vector> PhaseSpectrums { get => _phaseSpectrums; set => SetProperty(ref _phaseSpectrums, value, nameof(PhaseSpectrums)); }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, string propertyName)
        {
            if (Equals(field, newValue))
            {
                return false;
            }
            field = newValue;
            NotifyPropertyChanged(propertyName);
            return true;
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SignalsCount):
                case nameof(HarmonicsCount):
                case nameof(SmoothingType):
                    Update();
                    break;
            }
        }

        private void Update()
        {
            var signals = CreateSignals().ToList();
            var sineSpectrumsTask = Task.Run(() => Signal.CalcSineSpectrums(HarmonicsCount, signals).ToList());
            var cosineSpectrumsTask = Task.Run(() => Signal.CalcCosineSpectrums(HarmonicsCount, signals).ToList());
            var sineSpectrums = sineSpectrumsTask.GetAwaiter().GetResult();
            var cosineSpectrums = cosineSpectrumsTask.GetAwaiter().GetResult();

            var amplitudeSpectrumsTask = Task.Run(() => Signal.CalcAmplitudeSpectrums(sineSpectrums, cosineSpectrums).ToList());
            var phaseSpectrumsTask = Task.Run(() => Signal.CalcPhaseSpectrums(sineSpectrums, cosineSpectrums).ToList());

            var smoothedSignalsTask = Task.Run(() => CreateSmoothedSignals(signals).ToList());

            var amplitudeSpectrums = amplitudeSpectrumsTask.GetAwaiter().GetResult();
            var phaseSpectrums = phaseSpectrumsTask.GetAwaiter().GetResult();
            var smoothedSignals = smoothedSignalsTask.GetAwaiter().GetResult();

            Signals = signals.AsPoints();
            AmplitudeSpectrums = amplitudeSpectrums.AsPoints();
            PhaseSpectrums = phaseSpectrums.AsPoints();
            SmoothedSignals = smoothedSignals.AsPoints();
        }

        private IEnumerable<double> CreateSignals()
        {
            var random = new Random();
            bool RandomBool() => (random.Next() | 1) == 0;
            return Signal.CreateSignals(SignalsCount, RandomBool, 256, 16);
        }

        private IEnumerable<double> CreateSmoothedSignals(IEnumerable<double> signals)
        {
            switch(SmoothingType)
            {
                case SmoothingType.Parabolic:
                    var factors = new double[] { 110, -198, -135, 110, 390, 600, 677, 600, 390, 110, -135, -198, 110};
                    return Signal.ParabolicSmoothing(signals, factors, 2431);
                case SmoothingType.Sliding:
                    return Signal.SlidingSmoothing(signals, 9);
                case SmoothingType.Median:
                    return Signal.MedianSmoothing(signals, 5, 1);
                default:
                    return Enumerable.Empty<double>();
            }
        }
    }
}