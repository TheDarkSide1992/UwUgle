import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {DocumentSearchResult} from "./models/DocumentSearchResult.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "./Envirements/envirement";
import {FormsModule} from "@angular/forms";
import {NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule, NgForOf, NgIf],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Frontend';
  query: string = '';
  documentSearchResults: DocumentSearchResult[] = [];

  constructor(private http: HttpClient) {
  }

  async searchDocumentsTest(query: string) {
    let txty : DocumentSearchResult = {
      DocumentID : "this.q",
      DocumentName : query,
    }
    this.documentSearchResults.push(txty)

    console.log(query);
  }

  async getDocuement(DocumentID: string){
    console.log(DocumentID);
  }

  async searchDocuments(query: string) {
    try {
      const headers = new HttpHeaders({"Access-Control-Allow-Origin": "*", "Content-Type": "application/json", "Access-Control-Allow-Methods": "GET"});

      const call = this.http.get<DocumentSearchResult[]>(environment.baseURL + "query=" + query, {headers: headers});
      this.documentSearchResults = [];
      call.subscribe((resData: DocumentSearchResult[]) => {
        this.documentSearchResults.push(...resData);
      })
    } catch (error) {
    }

    console.log(this.documentSearchResults);

  }

}
