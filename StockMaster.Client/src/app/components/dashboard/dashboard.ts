import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth';
import { ProductService } from '../../services/product';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProductDto } from '../../models/product.dto';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent implements OnInit {

  products: ProductDto[] = [];

  constructor(
    private authService: AuthService,
    private productService: ProductService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getAll().subscribe({
      next: (res) => {
        this.products = res.data;
        console.log("Ürünler geldi:", this.products);
      },
      error: (err) => {
        console.error("Ürünler çekilemedi", err);
      }
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}