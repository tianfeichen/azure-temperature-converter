export enum Temperatures {
  Celsius = 'celsius',
  Fahrenheit = 'fahrenheit',
  Kelvin = 'kelvin'
}

export interface Temperature {
  [index: string]: number|null;
  celsius: number|null;
  fahrenheit: number|null;
  kelvin: number|null;
}
