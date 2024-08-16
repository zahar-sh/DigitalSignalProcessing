use std::{
    error::Error,
    f64::consts::{FRAC_PI_2, FRAC_PI_3, FRAC_PI_4, FRAC_PI_6, PI},
    ops::Range,
    path::Path,
};

use filter::{median_smooth, parabolic_smooth, sliding_smooth};
use fourier::{cos_sin_ampl, fast_fourier_transform, harm_ampl, phase_ampl, restore_signal};
use harmonic::{Harmonic, PolyHarmonic};
use plotters::{
    chart::ChartBuilder,
    prelude::{BitMapBackend, IntoDrawingArea, PathElement},
    series::LineSeries,
    style::{colors::*, Color, FontDesc, IntoFont, RGBColor, ShapeStyle},
};
use rand::{distributions::Bernoulli, prelude::Distribution, Rng};
use utils::{Duplicate, TupleMapper};

mod filter;
mod fourier;
mod harmonic;
mod utils;

fn harmonics_by_phase(ang_freq: f64) -> Vec<Harmonic> {
    let ampl = 6.0;
    let freq = 3.0;
    let phases = [2.0 * PI, FRAC_PI_6, FRAC_PI_2, 0.0, 3.0 * FRAC_PI_4];
    phases
        .into_iter()
        .map(move |phase| Harmonic::new(ampl, freq, ang_freq, phase))
        .collect()
}

fn harmonics_by_freq(ang_freq: f64) -> Vec<Harmonic> {
    let ampl = 8.0;
    let freqs = [2.0, 4.0, 3.0, 7.0, 5.0];
    let phase = FRAC_PI_4;
    freqs
        .into_iter()
        .map(move |freq| Harmonic::new(ampl, freq, ang_freq, phase))
        .collect()
}

fn harmonics_by_ampl(ang_freq: f64) -> Vec<Harmonic> {
    let ampls = [2.0, 5.0, 8.0, 3.0, 1.0];
    let freq = 5.0;
    let phase = FRAC_PI_4;
    ampls
        .into_iter()
        .map(move |ampl| Harmonic::new(ampl, freq, ang_freq, phase))
        .collect()
}

fn polyharmonic(ang_freq: f64) -> PolyHarmonic<Vec<Harmonic>> {
    let harmonics = vec![
        Harmonic::new(6.0, 1.0, ang_freq, FRAC_PI_6),
        Harmonic::new(6.0, 2.0, ang_freq, FRAC_PI_2),
        Harmonic::new(6.0, 3.0, ang_freq, FRAC_PI_3),
        Harmonic::new(6.0, 4.0, ang_freq, PI / 9.0),
        Harmonic::new(6.0, 5.0, ang_freq, 0.0),
    ];
    PolyHarmonic::new(harmonics)
}

fn save_charts<P, S, I, L, C>(
    path: &P,
    width: u32,
    height: u32,
    caption: &str,
    caption_font: FontDesc,
    label_font: FontDesc,
    series: S,
    range_x: Range<f64>,
    range_y: Range<f64>,
) -> Result<(), Box<dyn Error>>
where
    P: AsRef<Path> + ?Sized,
    S: Iterator<Item = (I, L, C)>,
    I: Iterator<Item = (f64, f64)>,
    L: Into<String>,
    C: Into<ShapeStyle> + Clone,
{
    let root = BitMapBackend::new(path, (width, height)).into_drawing_area();
    root.fill(&WHITE)?;
    let mut chart = ChartBuilder::on(&root)
        .caption(caption, caption_font)
        .margin(5)
        .x_label_area_size(30)
        .y_label_area_size(30)
        .build_cartesian_2d(range_x, range_y)?;
    chart.configure_mesh().draw()?;

    for (paris, label, color) in series {
        chart
            .draw_series(LineSeries::new(paris, color.clone()))?
            .label(label)
            .legend(move |(x, y)| PathElement::new(vec![(x, y), (x + 20, y)], color.clone()));
    }

    chart
        .configure_series_labels()
        .label_font(label_font)
        .background_style(&WHITE.mix(0.8))
        .border_style(&BLACK)
        .draw()?;

    Ok(())
}

fn colors() -> impl Iterator<Item = RGBColor> {
    let colors = [RED, GREEN, BLUE, YELLOW, CYAN, MAGENTA];
    colors.into_iter().cycle()
}

fn draw_harmonics_by_phase<P>(path: &P) -> Result<(), Box<dyn Error>>
where
    P: AsRef<Path> + ?Sized,
{
    let (width, height) = (1200, 600);
    let caption = "Harmonic signals by phase";
    let caption_font = ("Calibri", 20).into_font();
    let label_font = ("Calibri", 12).into_font();
    let n = 2048;
    let range_x = 0.0..n as f64;
    let range_y = -10.0..10.0;
    let ang_freq = Harmonic::ang_freq_from_period(n as f64);
    let series = harmonics_by_phase(ang_freq)
        .into_iter()
        .map(|harmonic| {
            let pairs = (0..n)
                .map(|x| x as f64)
                .duplicate()
                .map_second(move |x| harmonic.signal(x));
            pairs
        })
        .zip(colors())
        .enumerate()
        .map(|(index, (pairs, color))| (pairs, format!("{}", index + 1), color));
    save_charts(
        path,
        width,
        height,
        caption,
        caption_font,
        label_font,
        series,
        range_x,
        range_y,
    )
}

fn draw_harmonics_by_freq<P>(path: &P) -> Result<(), Box<dyn Error>>
where
    P: AsRef<Path> + ?Sized,
{
    let (width, height) = (1200, 600);
    let caption = "Harmonic signals by freq";
    let caption_font = ("Calibri", 20).into_font();
    let label_font = ("Calibri", 12).into_font();
    let n = 2048;
    let range_x = 0.0..n as f64;
    let range_y = -10.0..10.0;
    let ang_freq = Harmonic::ang_freq_from_period(n as f64);
    let series = harmonics_by_freq(ang_freq)
        .into_iter()
        .map(|harmonic| {
            let pairs = (0..n)
                .map(|x| x as f64)
                .duplicate()
                .map_second(move |x| harmonic.signal(x));
            pairs
        })
        .zip(colors())
        .enumerate()
        .map(|(index, (pairs, color))| (pairs, format!("{}", index + 1), color));
    save_charts(
        path,
        width,
        height,
        caption,
        caption_font,
        label_font,
        series,
        range_x,
        range_y,
    )
}

fn draw_harmonics_by_ampl<P>(path: &P) -> Result<(), Box<dyn Error>>
where
    P: AsRef<Path> + ?Sized,
{
    let (width, height) = (1200, 600);
    let caption = "Harmonic signals by ampl";
    let caption_font = ("Calibri", 20).into_font();
    let label_font = ("Calibri", 12).into_font();
    let n = 2048;
    let range_x = 0.0..n as f64;
    let range_y = -10.0..10.0;
    let ang_freq = Harmonic::ang_freq_from_period(n as f64);
    let series = harmonics_by_ampl(ang_freq)
        .into_iter()
        .map(|harmonic| {
            let pairs = (0..n)
                .map(|x| x as f64)
                .duplicate()
                .map_second(move |x| harmonic.signal(x));
            pairs
        })
        .zip(colors())
        .enumerate()
        .map(|(index, (pairs, color))| (pairs, format!("{}", index + 1), color));
    save_charts(
        path,
        width,
        height,
        caption,
        caption_font,
        label_font,
        series,
        range_x,
        range_y,
    )
}

fn draw_polyharmonic<P>(path: &P) -> Result<(), Box<dyn Error>>
where
    P: AsRef<Path> + ?Sized,
{
    let (width, height) = (1200, 600);
    let caption = "Polyharmonic signal";
    let caption_font = ("Calibri", 20).into_font();
    let label_font = ("Calibri", 12).into_font();
    let n = 2048;
    let range_x = 0.0..n as f64;
    let range_y = -20.0..30.0;
    let ang_freq = Harmonic::ang_freq_from_period(n as f64);
    let series = std::iter::once({
        let polyharmonic = polyharmonic(ang_freq);
        let pairs = (0..n)
            .map(|x| x as f64)
            .duplicate()
            .map_second(move |x| polyharmonic.signal(x));
        let label = "";
        let color = &RED;
        (pairs, label, color)
    });
    save_charts(
        path,
        width,
        height,
        caption,
        caption_font,
        label_font,
        series,
        range_x,
        range_y,
    )
}

fn draw_charts() -> Result<(), Box<dyn Error>> {
    let dir: &str = "./charts";
    std::fs::create_dir_all(dir)?;
    let ref phase_path = format!("{}/phase.png", dir);
    let ref freq_path = format!("{}/freq.png", dir);
    let ref ampl_path = format!("{}/ampl.png", dir);
    let ref polyharmonic_path = format!("{}/polyharmonic.png", dir);
    draw_harmonics_by_phase(phase_path)?;
    draw_harmonics_by_freq(freq_path)?;
    draw_harmonics_by_ampl(ampl_path)?;
    draw_polyharmonic(polyharmonic_path)?;
    Ok(())
}

fn calc_average_sq_and_dispersion<T>(items: T, m: usize) -> (f64, f64)
where
    T: Iterator<Item = f64>,
{
    let (sum, sum_sq) = items.fold((0.0, 0.0), |(sum, sum_sq), v| (sum + v, sum_sq + (v * v)));
    let f = 1.0 / (m + 1) as f64;
    let avg_sq = f * sum_sq;
    let avg = f * sum;
    let dispersion = avg_sq - avg * avg;
    (avg_sq, dispersion)
}

fn draw_chart_stat() -> Result<(), Box<dyn Error>> {
    let dir: &str = "./charts";
    std::fs::create_dir_all(dir)?;
    let ref path = format!("{}/signal-info.png", dir);
    let (width, height) = (1200, 600);
    let caption = "Signal statistic";
    let caption_font = ("Calibri", 20).into_font();
    let label_font = ("Calibri", 12).into_font();
    let n = 2048;
    let k = n / 4;
    let m = k + 1;
    let phases = [0.0, 2.0 * FRAC_PI_3];
    let range_x = 0.0..n as f64;
    let range_y = -2.0..2.0;
    let ang_freq = Harmonic::ang_freq_from_period(n as f64);
    let series = phases.into_iter().zip(colors()).map(move |(phase, color)| {
        let harmonic = Harmonic {
            phase,
            ang_freq,
            ..Default::default()
        };
        let pairs = (0..n)
            .map(|x| x as f64)
            .duplicate()
            .map_second(move |x| harmonic.signal(x))
            .collect::<Vec<_>>();
        let values = pairs.iter().map(|(_x, y)| y).copied();
        let (avg_sq, disp) = calc_average_sq_and_dispersion(values, m);
        let (avg_sq_val, dist_val) = (avg_sq.sqrt(), disp.sqrt());
        let (avg_sq_val_err, disp_val_err) = (0.707 - avg_sq_val, 0.707 - dist_val);
        let label = format!(
            "({:.5e},{:.5e})({:.5e},{:.5e})",
            avg_sq_val, avg_sq_val_err, dist_val, disp_val_err,
        );
        (pairs.into_iter(), label, color)
    });
    save_charts(
        path,
        width,
        height,
        caption,
        caption_font,
        label_font,
        series,
        range_x,
        range_y,
    )
}

fn draw_fourier_transform() -> Result<(), Box<dyn Error>> {
    let dir: &str = "./charts";
    std::fs::create_dir_all(dir)?;
    let ref path = format!("{}/fft.png", dir);
    let (width, height) = (1200, 600);
    let caption = "Fourier transform";
    let caption_font = ("Calibri", 20).into_font();
    let label_font = ("Calibri", 12).into_font();
    let n = 2048;
    let mid = n / 2 - 1;
    let range_x = 0.0..n as f64;
    let ang_freq = Harmonic::ang_freq_from_period(n as f64);
    let harmonic = Harmonic::new(10.0, 2.0, ang_freq, -FRAC_PI_2);
    let range_y = -harmonic.ampl..harmonic.ampl;
    let pairs = (0..n)
        .map(move |i| i as f64)
        .duplicate()
        .map_second(|x| harmonic.signal(x));
    let signal = pairs.collect::<Vec<_>>();
    let harmonic_info = (0..mid).map(|i| {
        let (ampl_cos, ampl_sin) = cos_sin_ampl(signal.iter().map(|&(_t, v)| v), n, i);
        let harm_ampl = harm_ampl(ampl_cos, ampl_sin);
        let phase_ampl = phase_ampl(ampl_cos, ampl_sin);
        (ampl_cos, ampl_sin, harm_ampl, phase_ampl)
    });
    let harmonic_ampl_phase_pairs = harmonic_info
        .map(|(_cos, _sin, ampl, phase)| (ampl, phase))
        .collect::<Vec<_>>();
    let restored_signal = {
        let time = (0..n).map(move |i| i as f64);
        restore_signal(
            time,
            harmonic_ampl_phase_pairs.iter().map(move |&v| v),
            ang_freq,
        )
    };
    let restored_signal_without_phase = {
        let time = (0..n).map(move |i| i as f64);
        restore_signal(
            time,
            harmonic_ampl_phase_pairs
                .iter()
                .map(move |&(ampl, _phase)| (ampl, 0.0)),
            ang_freq,
        )
    };
    let series = [
        ((signal.into_iter()), "Source"),
        (
            (restored_signal.collect::<Vec<_>>().into_iter()),
            "Restored",
        ),
        (
            (restored_signal_without_phase
                .collect::<Vec<_>>()
                .into_iter()),
            "Restored without phase",
        ),
    ]
    .into_iter()
    .zip(colors())
    .map(move |((data, label), color)| (data, label, color));
    save_charts(
        path,
        width,
        height,
        caption,
        caption_font,
        label_font,
        series,
        range_x,
        range_y,
    )
}

fn draw_fourier_transform_for_polyharmonic() -> Result<(), Box<dyn Error>> {
    let dir: &str = "./charts";
    std::fs::create_dir_all(dir)?;
    let ref path = format!("{}/fft-polyharmonic.png", dir);
    let (width, height) = (1200, 600);
    let caption = "Fourier transform for polyharmonic";
    let caption_font = ("Calibri", 20).into_font();
    let label_font = ("Calibri", 12).into_font();
    let n = 2048;
    let range_x = 0.0..n as f64;
    let range_y = -20.0..30.0;
    let ang_freq = Harmonic::ang_freq_from_period(n as f64);
    let signal = {
        let polyharmonic = polyharmonic(ang_freq);
        let pairs = (0..n)
            .map(|x| x as f64)
            .duplicate()
            .map_second(move |x| polyharmonic.signal(x));
        pairs.collect::<Vec<_>>()
    };
    let harmonic_ampl_phase_pairs =
        fast_fourier_transform(signal.iter().map(|(_x, y)| y).copied()).collect::<Vec<_>>();
    let restored_signal = {
        let time = (0..n).map(move |i| i as f64);
        restore_signal(
            time,
            harmonic_ampl_phase_pairs.iter().map(move |&v| v),
            ang_freq,
        )
    };
    let restored_signal_without_phase = {
        let time = (0..n).map(move |i| i as f64);
        restore_signal(
            time,
            harmonic_ampl_phase_pairs
                .iter()
                .map(move |&(ampl, _phase)| (ampl, 0.0)),
            ang_freq,
        )
    };
    let series = [
        ((signal.into_iter()), "Source"),
        (
            (restored_signal.collect::<Vec<_>>().into_iter()),
            "Restored",
        ),
        (
            (restored_signal_without_phase
                .collect::<Vec<_>>()
                .into_iter()),
            "Restored without phase",
        ),
    ]
    .into_iter()
    .zip(colors())
    .map(move |((data, label), color)| (data, label, color));
    save_charts(
        path,
        width,
        height,
        caption,
        caption_font,
        label_font,
        series,
        range_x,
        range_y,
    )
}

fn create_distorted_signal<T, R>(
    time: T,
    primary_harmonic: Harmonic,
    secondary_harmonic: Harmonic,
    secondary_range: Range<i32>,
    mut rng: R,
) -> impl Iterator<Item = (f64, f64)>
where
    R: Rng,
    T: Iterator<Item = f64>,
{
    let distribution = Bernoulli::new(0.5).unwrap();
    time.duplicate().map_second(move |x| {
        let primary: f64 = primary_harmonic.signal(x);
        let secondary: f64 = secondary_range
            .clone()
            .map(|i| {
                let signal = secondary_harmonic.signal(x * i as f64);
                let sign = distribution.sample(&mut rng);
                if sign {
                    signal
                } else {
                    -signal
                }
            })
            .sum();
        primary + secondary
    })
}

fn draw_signal_filtering() -> Result<(), Box<dyn Error>> {
    let dir: &str = "./charts";
    std::fs::create_dir_all(dir)?;
    let ref parabolic_path = format!("{}/parabolic-smooth.png", dir);
    let ref sliding_path = format!("{}/sliding-smooth.png", dir);
    let ref median_path = format!("{}/median-smooth.png", dir);
    let (width, height) = (1200, 600);
    let caption_font = ("Calibri", 20).into_font();
    let label_font = ("Calibri", 12).into_font();
    let n = 2048;
    let ang_freq = Harmonic::ang_freq_from_period(n as f64);
    let primary_harmonic = Harmonic::new(256.0, 1.0, ang_freq, 0.0);
    let secondary_harmonic = Harmonic::new(16.0, 1.0, ang_freq, 0.0);
    let (parabolic_div, parabolic_factors) = (
        2431.0,
        vec![
            110.0, -198.0, -135.0, 110.0, 390.0, 600.0, 677.0, 600.0, 390.0, 110.0, -135.0, -198.0,
            110.0,
        ],
    );
    let slid_size = 9;
    let (median_size, median_skip) = (5, 1);
    let range_x = 0.0..n as f64;
    let range_y = -300.0..300.0;
    let rng = rand::thread_rng();
    let time = (0..n).map(|i| i as f64);
    let pairs = create_distorted_signal(time, primary_harmonic, secondary_harmonic, 50..70, rng);
    let signal = pairs.collect::<Vec<_>>();
    let parabolic_smoothed_signal =
        parabolic_smooth(signal.iter().copied(), parabolic_div, parabolic_factors)
            .collect::<Vec<_>>();
    let series = [
        (signal.iter().copied(), "Source", &RED),
        (
            parabolic_smoothed_signal.iter().copied(),
            "Parabolic",
            &BLUE,
        ),
    ]
    .into_iter();
    save_charts(
        parabolic_path,
        width,
        height,
        "Parabolic",
        caption_font.clone(),
        label_font.clone(),
        series,
        range_x.clone(),
        range_y.clone(),
    )?;

    let sliding_smoothed_signal =
        sliding_smooth(signal.iter().copied(), slid_size).collect::<Vec<_>>();
    let series = [
        (signal.iter().copied(), "Source", &RED),
        (sliding_smoothed_signal.iter().copied(), "Sliding", &BLUE),
    ]
    .into_iter();
    save_charts(
        sliding_path,
        width,
        height,
        "Sliding",
        caption_font.clone(),
        label_font.clone(),
        series,
        range_x.clone(),
        range_y.clone(),
    )?;

    let parabolic_smoothed_signal =
        median_smooth(signal.iter().copied(), median_size, median_skip).collect::<Vec<_>>();
    let series = [
        (signal.iter().copied(), "Source", &RED),
        (parabolic_smoothed_signal.iter().copied(), "Median", &BLUE),
    ]
    .into_iter();
    save_charts(
        median_path,
        width,
        height,
        "Median",
        caption_font,
        label_font,
        series,
        range_x,
        range_y,
    )?;
    Ok(())
}

fn main() {
    draw_charts().unwrap();
    draw_chart_stat().unwrap();
    draw_fourier_transform().unwrap();
    draw_fourier_transform_for_polyharmonic().unwrap();
    draw_signal_filtering().unwrap();
}
