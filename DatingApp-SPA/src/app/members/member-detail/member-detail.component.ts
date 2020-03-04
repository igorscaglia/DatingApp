import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';
import { AlertifyService } from 'src/app/services/Alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private userService: UserService,
              private alertify: AlertifyService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

    this.activatedRoute.data.subscribe(data => {
      // resolvedUser é o nome do parâmetro criado em routes.ts
      this.user = data.resolvedUser;
    });

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide
      }
    ];

    // Inicia sem imagens
    this.galleryImages = this.loadImages();

  }

  loadImages() {
    const imageUrls = [];

    // criado com o snippet forof
    for (const photo of this.user.photos) {

      // Vamos criar um objeto galleryImage para cada photo.
      // Ver documentação oficial https://www.npmjs.com/package/ngx-gallery
      // Description e isMain não precisaria, mas vamos colocar mesmo assim ;-)
      imageUrls.push(
        {
          small: photo.url,
          medium: photo.url,
          big: photo.url,
          description: photo.description,
          isMain: photo.isMain
        }
      );
    }

    return imageUrls;
  }

}
