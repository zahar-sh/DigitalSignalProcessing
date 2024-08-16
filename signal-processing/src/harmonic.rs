use std::{f64::consts::PI, ops::Deref};

#[derive(Debug, Clone, PartialEq)]
pub struct Harmonic {
    pub ampl: f64,
    pub freq: f64,
    pub ang_freq: f64,
    pub phase: f64,
}

impl Harmonic {
    pub fn ang_freq_from_period(period: f64) -> f64 {
        2.0 * PI / period
    }

    pub const fn new(ampl: f64, freq: f64, ang_freq: f64, phase: f64) -> Self {
        Self {
            ampl,
            freq,
            ang_freq,
            phase,
        }
    }

    pub fn signal(&self, x: f64) -> f64 {
        self.ampl * (self.freq * x * self.ang_freq + self.phase).sin()
    }
}

impl Default for Harmonic {
    fn default() -> Self {
        Self {
            ampl: 1.0,
            freq: 1.0,
            ang_freq: Harmonic::ang_freq_from_period(1.0),
            phase: 0.0,
        }
    }
}

#[derive(Debug, Clone, PartialEq, Eq)]
pub struct PolyHarmonic<T>
where
    T: Deref<Target = [Harmonic]>,
{
    harmonics: T,
}

impl<T> PolyHarmonic<T>
where
    T: Deref<Target = [Harmonic]>,
{
    pub const fn new(harmonics: T) -> Self {
        Self { harmonics }
    }

    pub fn signal(&self, x: f64) -> f64 {
        self.harmonics
            .iter()
            .map(move |harmonic| harmonic.signal(x))
            .sum()
    }
}
