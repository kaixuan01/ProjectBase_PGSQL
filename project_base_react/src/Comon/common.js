import Swal from 'sweetalert2';

// Basic alert
export const showBasicAlert = (title, text, icon = 'info') => {
  Swal.fire({
    title,
    text,
    icon,
    confirmButtonText: 'OK',
  });
};

// Success alert
export const showSuccessAlert = (title = 'Success', text = '', timer = 1000) => {
  Swal.fire({
    title,
    text,
    icon: 'success',
    confirmButtonText: 'Great!',
    timer,  // Set the timeout in milliseconds
    timerProgressBar: true,  // Optional: show a progress bar indicating the time remaining
    willClose: () => {
      Swal.stopTimer(); // Optional: stops the timer if the user closes the alert manually
    }
  });
};

// Error alert
export const showErrorAlert = (title = 'Error', text = '') => {
  Swal.fire({
    title,
    text,
    icon: 'error',
    confirmButtonText: 'OK',
  });
};

// Warning/Confirmation alert with confirmation and cancel callbacks
export const showConfirmAlert = ({
  title = 'Are you sure?',
  text = '',
  confirmButtonText = 'Yes, do it!',
  cancelButtonText = 'No, cancel!',
  onConfirm = () => {},
  onCancel = () => {},
} = {}) => {
  Swal.fire({
    title,
    text,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText,
    cancelButtonText,
  }).then((result) => {
    if (result.isConfirmed) {
      onConfirm();
    } else if (result.dismiss === Swal.DismissReason.cancel) {
      onCancel();
    }
  });
};
