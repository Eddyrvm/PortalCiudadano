function showSuccessMessage(message) {
    Swal.fire({
        icon: 'success',
        title: '¡Éxito!',
        text: message,
        confirmButtonText: 'Aceptar',
        timer: 3000,
        timerProgressBar: true
    });
}

function showErrorMessage(message) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: message,
        confirmButtonText: 'Aceptar'
    });
}

function showWarningMessage(message) {
    Swal.fire({
        icon: 'warning',
        title: 'Advertencia',
        text: message,
        confirmButtonText: 'Aceptar'
    });
}

function showConfirmationMessage(title, text, confirmText = 'Sí', cancelText = 'No', callback) {
    Swal.fire({
        title: title,
        text: text,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: confirmText,
        cancelButtonText: cancelText
    }).then((result) => {
        if (result.isConfirmed) {
            callback(); // Llama a la función de callback si el usuario confirma
        }
    });
}