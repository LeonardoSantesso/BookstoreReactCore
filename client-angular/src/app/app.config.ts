import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideClientHydration } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { ErrorInterceptor } from '../core/interceptors/error.interceptor';
import { JwtInterceptor } from '../core/interceptors/jwt.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), 
    provideHttpClient(withInterceptorsFromDi()),  
    { provide:HTTP_INTERCEPTORS, useClass:ErrorInterceptor, multi:true },
    { provide:HTTP_INTERCEPTORS, useClass:JwtInterceptor, multi:true },
  ]
};
