import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ThemeSwitcherComponent } from '../shared/components/theme-switcher/theme-switcher.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ToastModule, ConfirmDialogModule, ThemeSwitcherComponent],
  template: `
    <router-outlet />
    <p-toast />
    <p-confirmDialog />
    <app-theme-switcher />
  `
})
export class App {}
