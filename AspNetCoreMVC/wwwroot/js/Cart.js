
class Cart {
    clickIncrement(button) {
        let data = this.getData(button);
        data.Amount++;
        this.postAmount(data);
    }

    clickDecrement(button) {
        let data = this.getData(button);
        data.Amount--;
        this.postAmount(data);
    }

    updateAmount(input) {
        let data = this.getData(input);
        this.postAmount(data);
    }

    getData(element) {
        let itemLine = $(element).parents('[item-id]'); // take the parents button with the attribute item-id
        let itemId = $(itemLine).attr('item-id'); // take the atribute value 
        let newAmount = $(itemLine).find('input').val(); //Find take the will get the elements below the hiearquia.

        var data = {
            id: itemId,
            Amount: newAmount
        }
        return data;
    }

    postAmount(data) {

        let token = $('[name=__RequestVerificationToken]').val();

        let headers = {};

        headers['RequestVerificationToken'] = token;

        $.ajax({
            url: '/order/updateamount',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: headers
        }).done(function (response) {
            let orderItem = response.orderItem;
            let orderLine = $('[item-id=' + orderItem.id + ']');
            orderLine.find('input').val(orderItem.amount);  

            orderLine.find('[subtotal]').html((orderItem.subtotal).toFixed(2).replace('.', ','));
            let cartViewModel = response.cartViewModel;
            $('[number-itens]').html('Total: ' + cartViewModel.itens.length + ' itens');
            $('[total]').html('Total: ' + cartViewModel.total.toFixed(2).replace('.', ','));

            if (orderItem.amount == 0) {
                orderLine.remove();
            }
        }); 
    }
}

var cart = new Cart();