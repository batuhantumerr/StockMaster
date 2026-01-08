import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { CustomResponse } from '../models/response.dto';
import { ProductDto } from '../models/product.dto';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpClient) {}

  getAll() {
    // URL sonuna /api/products ekleyerek istek atıyoruz.
    // Interceptor sayesinde Token otomatik eklenecek!
    return this.http.get<CustomResponse<ProductDto[]>>(`${environment.baseUrl}/products`);
  }
}