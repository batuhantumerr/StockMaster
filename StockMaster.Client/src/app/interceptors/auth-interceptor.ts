import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // 1. Tarayıcı hafızasından token'ı al
  const token = localStorage.getItem('token');

  // 2. Eğer token varsa, isteği kopyala ve içine Header ekle
  if (token) {
    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    // 3. Değiştirilmiş isteği yola devam ettir
    return next(clonedRequest);
  }

  // 4. Token yoksa isteği olduğu gibi gönder (Zaten Login sayfasına düşer)
  return next(req);
};