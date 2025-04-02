import { defineStore } from "pinia";
import { ref } from "vue";

export interface Toast {
  id: number;
  type: "success" | "error" | "info" | "warning" | "confirmation";
  message: string;
  details: string;
  duration: number;
  isClosable: boolean;
}

export const useToastStore = defineStore("toast", () => {
  const toasts = ref<Toast[]>([]);

  const addToast = (
    message: string,
    details: string,
    type: "success" | "error" | "info" | "warning" | "confirmation",
    duration: number = 5000,
    isClosable: boolean = true
  ) => {
    const toast: Toast = {
      id: new Date().getTime(),
      type: type,
      message: message,
      details: details,
      duration: duration,
      isClosable: isClosable,
    };
    toasts.value.push(toast);

    setTimeout(() => removeToast(toast.id), duration);
  };
  const removeToast = (id: number) => {
    const index = toasts.value.findIndex((toast) => toast.id === id);
    if (index !== -1) {
      toasts.value.splice(index, 1);
      // toasts.value = [...toasts];
    }
  };

  return { toasts, addToast, removeToast };
});
