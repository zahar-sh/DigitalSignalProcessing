use std::f64::consts::PI;

use num::Complex;

use crate::harmonic::Harmonic;

pub fn cos_sin_ampl<T>(signal: T, n: usize, harm: usize) -> (f64, f64)
where
    T: Iterator<Item = f64>,
{
    let ang_freq = Harmonic::ang_freq_from_period(n as f64);
    let sin_table = (0..n)
        .map(move |i| (i as f64 * ang_freq).sin())
        .collect::<Vec<_>>();
    let (sum_re, sum_im) = signal
        .into_iter()
        .enumerate()
        .map(move |(i, t)| {
            let sin_index = (i * harm) % n;
            let cos_index = (i * harm + n / 4) % n;
            let sin = sin_table[sin_index];
            let cos = sin_table[cos_index];
            let re = t * cos;
            let im = t * sin;
            (re, im)
        })
        .fold((0.0, 0.0), |(sum_re, sum_im), (re, im)| {
            (sum_re + re as f64, sum_im + im as f64)
        });
    let a = 2.0 / (n as f64);
    let ampl_cos = (sum_re * a) as f64;
    let ampl_sin = (sum_im * a) as f64;
    (ampl_cos, ampl_sin)
}

pub fn harm_ampl(ampl_cos: f64, ampl_sin: f64) -> f64 {
    (ampl_cos * ampl_cos + ampl_sin * ampl_sin).sqrt()
}

pub fn phase_ampl(ampl_cos: f64, ampl_sin: f64) -> f64 {
    ampl_sin.atan2(ampl_cos)
}

pub fn fast_fourier_transform<T>(signal: T) -> impl Iterator<Item = (f64, f64)>
where
    T: Iterator<Item = f64>,
{
    fn apply_fourier_transform(complexes: &mut [Complex<f64>]) {
        let n = complexes.len();
        if n < 2 {
            return;
        }
        let mut even = complexes.iter().step_by(2).map(|&c| c).collect::<Vec<_>>();
        let mut odd = complexes
            .iter()
            .skip(1)
            .step_by(2)
            .map(|&c| c)
            .collect::<Vec<_>>();

        apply_fourier_transform(even.as_mut_slice());
        apply_fourier_transform(odd.as_mut_slice());

        let ang_freq =
            Complex::new(0.0, -2.0) * Complex::new(PI, 0.0) / Complex::new(n as f64, 0.0);
        for (index, (dest, (e, o))) in complexes
            .iter_mut()
            .zip(even.iter().zip(odd.iter()))
            .enumerate()
        {
            let ang = (Complex::new(index as f64, 0.0) * ang_freq).exp();
            *dest = *e + ang * o;
        }

        let mid = n / 2;
        for (index, (dest, (o, e))) in complexes[mid..]
            .iter_mut()
            .zip(odd.iter().zip(even.iter()))
            .enumerate()
        {
            let ang = (Complex::new((index + mid) as f64, 0.0) * ang_freq).exp();
            *dest = *e + ang * o;
        }
    }

    let mut complexes = signal
        .map(move |v| Complex::new(v, 0.0))
        .collect::<Vec<_>>();

    apply_fourier_transform(complexes.as_mut_slice());

    let n = complexes.len();
    return complexes.into_iter().map(move |v| {
        let ampl = v.norm_sqr().sqrt() / n as f64;
        let phase = -v.im.atan2(v.re);
        (ampl, phase)
    });
}

pub fn restore(ampl: f64, freq: f64, ang_freq: f64, phase: f64) -> f64 {
    ampl * (freq * ang_freq - phase).cos()
}

pub fn restore_signal<T, P>(
    time: T,
    harm_ampl_phase_pairs: P,
    ang_freq: f64,
) -> impl Iterator<Item = (f64, f64)>
where
    T: Iterator<Item = f64>,
    P: Iterator<Item = (f64, f64)> + Clone,
{
    time.map(move |t| {
        let v = harm_ampl_phase_pairs
            .clone()
            .enumerate()
            .map(move |(harm, (ampl, phase))| restore(ampl, t * harm as f64, ang_freq, phase))
            .sum();
        (t, v)
    })
}
