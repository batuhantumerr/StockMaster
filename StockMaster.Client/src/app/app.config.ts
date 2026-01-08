import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';

import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import { authInterceptor } from './interceptors/auth-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),

    // 1. HTTP İsteklerini Açıyoruz (Fetch API ile)
    provideHttpClient(
      withFetch(),
      withInterceptors([authInterceptor])
    ),

    // 2. Animasyonları Açıyoruz (Toastr için gerekli)
    provideAnimations(),

    // 3. Toastr Bildirimlerini Ayarlıyoruz
    provideToastr({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      progressBar: true
    })
  ]
};