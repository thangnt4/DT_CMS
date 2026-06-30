import { Injectable, signal } from '@angular/core';
import { updatePrimaryPalette, updateSurfacePalette } from '@primeng/themes';

export interface ThemeColor {
  name: string;
  swatch: string;
}

export const PRIMARY_COLORS: ThemeColor[] = [
  { name: 'emerald', swatch: '#10b981' },
  { name: 'green', swatch: '#22c55e' },
  { name: 'lime', swatch: '#84cc16' },
  { name: 'orange', swatch: '#f97316' },
  { name: 'amber', swatch: '#f59e0b' },
  { name: 'yellow', swatch: '#eab308' },
  { name: 'teal', swatch: '#14b8a6' },
  { name: 'cyan', swatch: '#06b6d4' },
  { name: 'sky', swatch: '#0ea5e9' },
  { name: 'blue', swatch: '#3b82f6' },
  { name: 'indigo', swatch: '#6366f1' },
  { name: 'violet', swatch: '#8b5cf6' },
  { name: 'purple', swatch: '#a855f7' },
  { name: 'fuchsia', swatch: '#d946ef' },
  { name: 'pink', swatch: '#ec4899' },
  { name: 'rose', swatch: '#f43f5e' }
];

export const SURFACE_COLORS: ThemeColor[] = [
  { name: 'slate', swatch: '#64748b' },
  { name: 'gray', swatch: '#6b7280' },
  { name: 'zinc', swatch: '#71717a' },
  { name: 'neutral', swatch: '#737373' },
  { name: 'stone', swatch: '#78716c' }
];

const SHADES = [50, 100, 200, 300, 400, 500, 600, 700, 800, 900, 950];
const STORAGE_KEY = 'dt_cms_theme';

interface StoredTheme {
  primary: string;
  surface: string;
  dark: boolean;
}

function buildPalette(colorName: string): Record<number, string> {
  return Object.fromEntries(SHADES.map((shade) => [shade, `{${colorName}.${shade}}`]));
}

@Injectable({ providedIn: 'root' })
export class ThemeService {
  readonly primaryColor = signal('indigo');
  readonly surfaceColor = signal('slate');
  readonly darkMode = signal(false);
  readonly panelVisible = signal(false);

  openPanel(): void {
    this.panelVisible.set(true);
  }

  constructor() {
    const stored = this.readStored();
    this.primaryColor.set(stored.primary);
    this.surfaceColor.set(stored.surface);
    this.darkMode.set(stored.dark);

    updatePrimaryPalette(buildPalette(stored.primary));
    updateSurfacePalette(buildPalette(stored.surface));
    this.applyDarkModeClass(stored.dark);
  }

  setPrimaryColor(colorName: string): void {
    this.primaryColor.set(colorName);
    updatePrimaryPalette(buildPalette(colorName));
    this.persist();
  }

  setSurfaceColor(colorName: string): void {
    this.surfaceColor.set(colorName);
    updateSurfacePalette(buildPalette(colorName));
    this.persist();
  }

  toggleDarkMode(): void {
    const next = !this.darkMode();
    this.darkMode.set(next);
    this.applyDarkModeClass(next);
    this.persist();
  }

  private applyDarkModeClass(enabled: boolean): void {
    document.documentElement.classList.toggle('app-dark', enabled);
  }

  private persist(): void {
    const value: StoredTheme = {
      primary: this.primaryColor(),
      surface: this.surfaceColor(),
      dark: this.darkMode()
    };
    localStorage.setItem(STORAGE_KEY, JSON.stringify(value));
  }

  private readStored(): StoredTheme {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) {
      return { primary: 'indigo', surface: 'slate', dark: false };
    }
    try {
      return { primary: 'indigo', surface: 'slate', dark: false, ...JSON.parse(raw) };
    } catch {
      return { primary: 'indigo', surface: 'slate', dark: false };
    }
  }
}
