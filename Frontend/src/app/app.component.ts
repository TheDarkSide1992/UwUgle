import {Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {DocumentSimple} from "./models/DocumentSimple.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "./Envirements/envirement";
import {FormsModule} from "@angular/forms";
import {NgForOf, NgIf} from "@angular/common";
import {Observable, throwError} from "rxjs";

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
  documentSearchResults: DocumentSimple[] = [];

  constructor(private http: HttpClient) {
  }

  getDocuement(DocumentID: number): Observable<Blob> {
      return this.http.get(environment.baseURL+"/"+DocumentID, { responseType: 'blob' });
  }


  download(DocumentID: number, FileName:string) {
    this.getDocuement(DocumentID).subscribe((blob) => {
      const a = document.createElement('a');
      const objectUrl = URL.createObjectURL(blob);
      a.href = objectUrl;
      a.download = FileName+".txt";
      a.click();
      URL.revokeObjectURL(objectUrl);
    });
  }


  async searchDocuments(query: string) {
    try {
      this.documentSearchResults = []
      const call = this.http.get<DocumentSimple[]>(environment.baseURL + "?query=" + query);
      call.subscribe((resData: DocumentSimple[]) => {
        this.documentSearchResults = resData;
      })
    } catch (error) {
      // @ts-ignore
      if (error.status === 404) {
        console.log("could not find resposne for " + query);
      } else {
        console.error("Unexpected error " + error);
      }
    }
  }

}
