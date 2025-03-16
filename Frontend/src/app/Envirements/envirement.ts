export const environment = {
  production: false,
  baseURL: window.location.hostname === 'localhost'
    ? 'http://localhost:8000/SearchEngine' // Local dev
    : 'http://apiservice:8000/SearchEngine' // Docker internal network
};
