import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { ErrorHandlerService } from 'src/app/services/error-handler.service';
import { AlertifyService } from 'src/app/services/Alertify.service';

// Esse componente é filho do member-edit

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  // photos passadas pelo member-edit
  @Input() photos: Photo[];
  // Evento que vai disparar quando a foto for alterada
  @Output() memberPhotoChangedEvent = new EventEmitter<string>();
  uploader: FileUploader; // No app.module tem que importar FileUploadModule
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMainPhoto: Photo;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService,
    private errorHandler: ErrorHandlerService
  ) { }

  ngOnInit() {
    this.initializeUploader();
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + this.authService.getToken(),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    // Para não dar erro de CORS
    this.uploader.onAfterAddingFile = (file) => file.withCredentials = false;

    this.uploader.onSuccessItem = (item, response, status, header) => {
      if (response) {

        const photoResponse: Photo = JSON.parse(response);
        /*
                const photo = {
                  id: photoResponse.id,
                  url: photoResponse.url,
                  dateAdded: photoResponse.dateAdded,
                  description: photoResponse.description,
                  isMain: photoResponse.isMain
                };
        */
        // Adicionamos a photo de resposta no array para atualizar a lista e assim a UI
        this.photos.push(photoResponse);
      }
    };
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  setMainPhoto(photo: Photo) {
    this.userService.setMainPhoto(this.authService.decodedToken.nameid, photo.id).subscribe(() => {

      // Vamos atualizar a UI com a photo nova
      this.currentMainPhoto = this.photos.filter(p => p.isMain === true)[0];
      this.currentMainPhoto.isMain = false;
      photo.isMain = true;

      // Vamos disparar o evento passando a url da imagem da nova photo principal
      this.memberPhotoChangedEvent.emit(photo.url);

      console.log('Success main photo change.');
    }, error => {
      this.alertify.error(this.errorHandler.handle(error));
    });
  }

}
