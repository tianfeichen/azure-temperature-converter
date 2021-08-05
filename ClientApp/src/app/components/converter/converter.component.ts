import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';;
import { Temperature, Temperatures } from 'src/app/models/temperature';
import { TemperatureService } from 'src/app/services/temperature.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-converter',
  templateUrl: './converter.component.html',
  styleUrls: ['./converter.component.css']
})
export class ConverterComponent implements OnInit {

  temperature: Temperature = {
    celsius: null,
    fahrenheit: null,
    kelvin: null
  };

  temperatureForm = new FormGroup({
    celsius: new FormControl(''),
    fahrenheit: new FormControl(''),
    kelvin: new FormControl('')
  });

  isLoading = false;
  errorMessage = '';

  constructor(private temperatureService: TemperatureService) {
  }

  ngOnInit(): void {
    Object.values(Temperatures).forEach(temp => {
      this.temperatureForm.controls[temp].valueChanges
        .pipe(
          debounceTime(400),
          distinctUntilChanged()
        )
        .subscribe(value => {
          this.convert(value, temp);
        });
    });
  }

  convert(temperature: number, from: Temperatures): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.temperatureForm.disable({onlySelf: true, emitEvent: false});

    this.temperatureService.convert(temperature, from)
      .subscribe(
        temp => {
          this.temperature = temp;
          this.temperatureForm.setValue(temp, {emitEvent: false});
        },
        error => {
          this.temperatureForm.enable({onlySelf: true, emitEvent: false});
          this.errorMessage = `Failed to make request to server ${environment.apiUrl}. You can update it in environment file.`;
        }
      )
      .add(() => {
          this.temperatureForm.enable({onlySelf: true, emitEvent: false});
          this.isLoading = false;
        }
      );
  }

}
