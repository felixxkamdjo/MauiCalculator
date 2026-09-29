# openCalc (MauiCalculator)

A modern, fluent, and intuitive cross-platform calculator and unit converter built with **.NET MAUI** (targeting Android). Designed with a sleek dark aesthetic inspired by Microsoft Fluent Design and MIUI/HyperOS interfaces.

---

## Overview

**openCalc** blends essential arithmetic calculation with advanced scientific functions and a responsive unit converter. The interface avoids system distractions (hidden default Shell navigation and tab bars) to deliver an immersive tactile experience with custom modals, responsive grid scaling, and live cursor indicators.

---

## Features

### 1. Core & Scientific Calculator
- **Basic Operations:** Addition (`+`), subtraction (`-`), multiplication (`×`), and division (`÷`).
- **Input Controls:** Full reset (`C`), character deletion (`⌫`), percentages (`%`), sign negation (`±`), and decimals (`.`).
- **Scientific Extensions:**
  - Square root ($\sqrt{x}$) with invalid domain handling.
  - Power squaring ($x^2$).
  - Reciprocal inversion ($1/x$).
  - Mathematical constant $\pi$.
- **Edge-Case Resilience:** Non-crashing inline handling of zero division and negative square roots.
- **In-Line Scrollable History:** Previous operations stack vertically above the active formula. Tapping any past equation restores its result.
- **Custom Dark Modal:** A custom pop-up dialog replacing default system action sheets to confirm clearing history logs.

### 2. Physical Unit Converter
- **Instant Category Switching:** Fast horizontal pills to toggle between:
  - **Length:** Meter (m), Kilometer (km), Centimeter (cm), Millimeter (mm), Mile (mi).
  - **Mass:** Kilogram (kg), Gram (g), Milligram (mg), Pound (lb).
  - **Temperature:** Celsius (°C), Fahrenheit (°F), Kelvin (K).
- **Embedded Tactile Keypad:** Dedicated keypad eliminating native soft keyboard pop-up delays.
- **Smart Input Validation:**
  - The negation toggle (`±`) is dynamically dimmed and disabled for physical dimensions that cannot be negative (Length, Mass).
  - An informative custom dialog notifies the user upon invalid attempts.
- **Clean Dropdowns:** Clean native spinners with platform underline removal for seamless UI balance.

---

## UI / UX Architecture

The application strictly implements .NET MAUI layout containers:
- **`Grid`:** High-density keypads and balanced dual-pane converter cards.
- **`VerticalStackLayout` & `HorizontalStackLayout`:** Natural vertical calculation feeds and navigation pill bars.
- **`ScrollView`:** Horizontal scrolling for oversized numbers to prevent text clipping, combined with vertical scrolling for calculation logs.
- **`Border`:** Modern rounded shapes with high-contrast borders matching the .NET Violet (`#512BD4`) design system.

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- .NET MAUI workload (`dotnet workload install maui-android`)
- Android SDK Platform-Tools (`adb`)

### Installation & Run

1. **Clone the repository:**
   ```bash
   git clone https://github.com/felixxkamdjo/MauiCalculator.git
   cd MauiCalculator
    ```

2. **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3. **Deploy to a connected Android device:**
    ```bash
    dotnet build -t:Run -f net10.0-android
    ```