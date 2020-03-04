import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersComponent } from './members/members.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './resolvers/member-detail.resolver';
import { MembersResolver } from './resolvers/members.resolver';

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
            { path: 'members', component: MembersComponent,
                resolve: {resolvedUsers: MembersResolver} },
            { path: 'members/:id', component: MemberDetailComponent,
                resolve: {resolvedUser: MemberDetailResolver} },
            { path: 'messages', component: MessagesComponent },
            { path: 'lists', component: ListsComponent }
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
