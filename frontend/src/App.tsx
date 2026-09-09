import "@/styles/global.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { AppRouter } from "@/components/AppRouter";

const queryClient = new QueryClient();

export function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <AppRouter />
    </QueryClientProvider>
  );
}

export default App;
