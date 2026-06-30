import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DrawerModule } from 'primeng/drawer';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { FormsModule } from '@angular/forms';
import { PRIMARY_COLORS, SURFACE_COLORS, ThemeService } from '../../../core/services/theme.service';

@Component({
  selector: 'app-theme-switcher',
  standalone: true,
  imports: [CommonModule, FormsModule, DrawerModule, ToggleSwitchModule],
  templateUrl: './theme-switcher.component.html'
})
export class ThemeSwitcherComponent {
  readonly themeService = inject(ThemeService);

  readonly primaryColors = PRIMARY_COLORS;
  readonly surfaceColors = SURFACE_COLORS;
}
