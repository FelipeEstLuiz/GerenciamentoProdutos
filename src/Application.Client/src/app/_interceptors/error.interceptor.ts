import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoadingService } from '../_services/loading.service';
import { HttpErrorResponse, HttpEvent } from '@angular/common/http';
import { catchError, finalize, map, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { AlertService } from '../_services/alert.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  const router = inject(Router);
  const accountService = inject(AccountService);
  const alertService = inject(AlertService);

  loadingService.show();

  return next(req).pipe(
    map((event: HttpEvent<any>) => {
      
      return event;
    }),
    catchError((error: HttpErrorResponse) => {
      const responseBody = error.error;

      let msg = 'Erro interno do servidor. Por favor, tente novamente mais tarde.';

      if (error.status === 401) {    

        accountService.logout?.();
        localStorage.removeItem('user');
        router.navigate(['/']);
        msg = 'Sessão expirada. Faça login novamente.';

      } else if (error.status !== 0) {  

        if (Array.isArray(responseBody)) {
          msg = responseBody.join('\n');
        } else if (responseBody && typeof responseBody === 'object' && 'errors' in responseBody) {
          msg = (responseBody.errors || []).join('\n') || msg;
        } else {
          msg = error.message || error.statusText || msg;
        }        
      } 

      alertService.error(msg);
      return throwError(() => msg);
    }),
    finalize(() => {
      loadingService.hide();
    })
  );
};
