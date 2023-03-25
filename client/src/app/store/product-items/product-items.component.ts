import { Component, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { IProduct } from 'src/app/shared/models/product';

@Component({
  selector: 'app-product-items',
  templateUrl: './product-items.component.html',
  styleUrls: ['./product-items.component.scss']
})
export class ProductItemsComponent {
  @Input() product?: IProduct;

  constructor(private basketService: BasketService){}

  addItemToBasket(){
    this.product && this.basketService.addItemToBasket(this.product);
  }
}
