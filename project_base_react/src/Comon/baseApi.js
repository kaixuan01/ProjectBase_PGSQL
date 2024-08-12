// export const httpPost = async (url, body = null) => {
//     const options = {
//       method: 'POST',
//       headers: {
//         'Content-Type': 'application/json',
//       },
//     };
//     if (body) {
//       options.body = JSON.stringify(body);
//     }
//     try {
//       const response = await fetch(url, options);
//       if (!response.ok) {
//         throw new Error(`HTTP error! Status: ${response.status}`);
//       }
//       return await response.json();
//     } catch (error) {
//       console.error('Error fetching data:', error);
//       throw error;
//     }
//   };

//   export const httpGet = async (url, body = null) => {
//     const options = {
//       method: 'GET',
//       headers: {
//         'Content-Type': 'application/json',
//       },
//     };
  
//     if (body) {
//       options.body = JSON.stringify(body);
//     }
  
//     try {
//       const response = await fetch(url, options);
//       if (!response.ok) {
//         throw new Error(`HTTP error! Status: ${response.status}`);
//       }
//       return await response.json();
//     } catch (error) {
//       console.error('Error fetching data:', error);
//       throw error;
//     }
//   };