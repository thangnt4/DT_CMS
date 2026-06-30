import { Routes } from '@angular/router';

export const systemRoutes: Routes = [
  {
    path: 'users',
    loadComponent: () => import('./users/users.component').then((m) => m.UsersComponent)
  }
];
