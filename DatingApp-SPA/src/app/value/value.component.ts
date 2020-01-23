import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {

  values: any;

  // os parâmetros passados pelo construtor são também propriedades
  // O injetor de dependência do angular se encarrega de carregar o objeto, já que
  // ele foi configurado em app.module
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  getValues() {
    // Todo observable precisa ser subscribe para que o resultado possa ser recuperado
    this.http.get('http://localhost:5000/api/values').subscribe( response => {
      this.values = response;
    }, error => {
      console.log(error);
    });
  }
}
