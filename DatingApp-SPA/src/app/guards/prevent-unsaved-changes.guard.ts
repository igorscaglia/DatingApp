import { Injectable } from '@angular/core';
import { CanDeactivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/Alertify.service';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

// Essa classe é chamada pelo mecanismo de rotas

@Injectable({
    providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {

    constructor(private alertify: AlertifyService) { }

    canDeactivate(component: MemberEditComponent): boolean {

        if (component.editForm.dirty) {
            return confirm('Are you sure? Any unsaved changes will be lost!');
        }
        return true;
    }
}
