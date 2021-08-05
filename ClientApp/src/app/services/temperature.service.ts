import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Temperature, Temperatures } from '../models/temperature';

@Injectable({
  providedIn: 'root'
})
export class TemperatureService {

  private url = `${environment.apiUrl}/temperature`;

  constructor(private http: HttpClient) {

  }

  convert(temperature: number, from: Temperatures): Observable<Temperature> {
    if (!temperature) {
      return of({celsius: null, fahrenheit: null, kelvin: null});
    }

    const url = `${this.url}/convert/${temperature}/${from}`;
    return this.http.get<Temperature>(url);
  }

}
