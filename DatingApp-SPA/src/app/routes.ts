import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersComponent } from './members/members.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './guards/auth.guard';

// Aqui vai conter todas as rotas da aplicação.
// Esse arquivo foi gerado manualmente.
// appRoutes é um array de rotas que usamos na aplicação
// A última rota é caso não achar nenhuma configurada, então volta pra home
// // localhost:4200/members... se tivesse algo no path seria /xxxmembers

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MembersComponent },
            { path: 'messages', component: MessagesComponent },
            { path: 'lists', component: ListsComponent }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
