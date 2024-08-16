use std::collections::VecDeque;

pub fn parabolic_smooth<T>(
    signal: T,
    divider: f64,
    factors: Vec<f64>,
) -> impl Iterator<Item = (f64, f64)>
where
    T: Iterator<Item = (f64, f64)>,
{
    let window_size = factors.len();
    let mut window = VecDeque::with_capacity(window_size);
    window.resize(window_size, 0.0f64);
    signal.map(move |(t, v)| {
        window.pop_front();
        window.push_back(v);
        let v = factors
            .iter()
            .zip(window.iter())
            .map(|(&a, &b)| a * b)
            .sum::<f64>()
            / divider;
        (t, v)
    })
}

pub fn sliding_smooth<T>(signal: T, window_size: usize) -> impl Iterator<Item = (f64, f64)>
where
    T: Iterator<Item = (f64, f64)>,
{
    let mut window = VecDeque::with_capacity(window_size);
    window.resize(window_size, 0.0f64);
    signal.map(move |(t, v)| {
        window.pop_front();
        window.push_back(v);
        let v = window.iter().sum::<f64>() / window_size as f64;
        (t, v)
    })
}

pub fn median_smooth<T>(
    signal: T,
    window_size: usize,
    skip: usize,
) -> impl Iterator<Item = (f64, f64)>
where
    T: Iterator<Item = (f64, f64)>,
{
    let mut window = VecDeque::with_capacity(window_size);
    window.resize(window_size, 0.0f64);
    let mut sorted_window = Vec::with_capacity(window_size);
    let divider = window_size - 2 * skip;
    signal.map(move |(t, v)| {
        window.pop_front();
        window.push_back(v);
        let (a, b) = window.as_slices();
        sorted_window.resize(0, 0.0);
        sorted_window.extend_from_slice(a);
        sorted_window.extend_from_slice(b);
        sorted_window.sort_by(|a, b| a.partial_cmp(b).unwrap());
        let v = sorted_window[skip..window_size - skip].iter().sum::<f64>() / divider as f64;
        (t, v)
    })
}
