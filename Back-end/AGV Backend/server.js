const express = require('express');
const app = express();
const port = 3000;


const MongoClient = require('mongodb').MongoClient;


// Enable command monitoring for debugging
const client = new MongoClient('mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+1.9.0');


app.get('/camera', async (req, res) => {
   try {
      const collection = client.db('AGV').collection('agv_camera');
      const projection = { data: 1, timestamp: 1, _id: 0 };
      const pipeline = [
      { $sort: { timestamp: -1 } },
      { $limit: 5 },
      { $project: projection }
      ];

      const data = await collection.aggregate(pipeline).toArray();

      const imageData = data.map(d => ({
        data: d.data,
        timestamp: new Date(d.timestamp)
      }));

      return res.json({ images_data: imageData });
    } catch (error) {
      console.error(error);
      return res.status(500).send('Internal Server Error');
    }
  });

  app.get('/line_tracker', async (req, res) => {
    try {
       const collection = client.db('AGV').collection('agv_line_tracker');
       const projection = { line_status: 1, timestamp: 1, _id: 0 };
       
         // Use the aggregation pipeline to optimize performance
       const pipeline = [
       { $sort: { timestamp: -1 } },
       { $limit: 5 },
       { $project: projection }
       ];
 
       const data = await collection.aggregate(pipeline).toArray();
 
       const lineTrackerData = data.map(d => ({
         line_status: d.line_status,
         timestamp: new Date(d.timestamp)
       }));
 
       // Return the sensor data in the response
       return res.json({ line_status_data: lineTrackerData });
     } catch (error) {
       console.error(error);
       return res.status(500).send('Internal Server Error');
     }
   });

   app.get('/obstacles_detected', async (req, res) => {
    try {
       const collection = client.db('AGV').collection('agv_obstacles_detected');
       const projection = { data: 1, timestamp: 1, _id: 0 };
        // Use the aggregation pipeline to optimize performance
       const pipeline = [
       { $sort: { timestamp: -1 } },
       { $limit: 5 },
       { $project: projection }
       ];
 
       const data = await collection.aggregate(pipeline).toArray();
 
       const imageData = data.map(d => ({
         data: d.data,
         timestamp: new Date(d.timestamp)
       }));
 
       return res.json({ images_data: imageData });
     } catch (error) {
       console.error(error);
       return res.status(500).send('Internal Server Error');
     }
   });
 
   app.get('/ultrasonic', async (req, res) => {
     try {
        const collection = client.db('AGV').collection('agv_ultrasonic');
        const projection = { left_distance: 1, right_distance: 1, back_distance: 1,front_distance: 1, timestamp: 1, _id: 0 };
        
          // Use the aggregation pipeline to optimize performance
        const pipeline = [
        { $sort: { timestamp: -1 } },
        { $limit: 5 },
        { $project: projection }
        ];
  
        const data = await collection.aggregate(pipeline).toArray();
        const leftDistance = data.map(d => ({ left_distance: d.left_distance, timestamp: new Date(d.timestamp) }));
        const rightDistance = data.map(d => ({ right_distance: d.right_distance, timestamp: new Date(d.timestamp) }));
        const backDistance = data.map(d => ({ back_distance: d.back_distance, timestamp: new Date(d.timestamp) }));
        const frontDistance = data.map(d => ({ front_distance: d.front_distance, timestamp: new Date(d.timestamp) }));
  
        // Return the sensor data in the response
        return res.json({ left_distance_data: leftDistance,
            right_distance_data: rightDistance,
            back_distance_data: backDistance,
            front_distance_data: frontDistance
        });
      } catch (error) {
        console.error(error);
        return res.status(500).send('Internal Server Error');
      }
    });
    app.get('/resources', async (req, res) => {
        try {
           const collection = client.db('AGV').collection('agv_resources');
           const projection = { memoryConsumed: 1,memoryAvailable: 1, cpuUtilization: 1, timestamp: 1, _id: 0 };
           const pipeline = [
           { $sort: { timestamp: -1 } },
           { $limit: 1 },
           { $project: projection }
           ];
     
           const data = await collection.aggregate(pipeline).toArray();
     
           const { memoryConsumed, memoryAvailable, cpuUtilization, timestamp } = data[0];

            // Return the memory and CPU utilization values in the response
           return res.json({ memory: {
            consumed: memoryConsumed,
            available: memoryAvailable
          },
          cpu: {
            utilization: cpuUtilization
          },
          timestamp: timestamp });
         } catch (error) {
           console.error(error);
           return res.status(500).send('Internal Server Error');
         }
       });
     
       app.get('/wheels', async (req, res) => {
         try {
            const collection = client.db('AGV').collection('agv_wheels');
            const projection = { direction: 1, speed: 1, timestamp: 1, _id: 0 };
            
              // Use the aggregation pipeline to optimize performance
            const pipeline = [
            { $sort: { timestamp: -1 } },
            { $limit: 5 },
            { $project: projection }
            ];
      
            const data = await collection.aggregate(pipeline).toArray();
      
            const speedData = data.map(d => ({ speed: d.speed, timestamp: new Date(d.timestamp) }));
            const directionData = data.map(d => ({ direction: d.direction, timestamp: new Date(d.timestamp) }));
      
            // Return the sensor data in the response
            return res.json({speed_data: speedData,
                direction_data: directionData });
          } catch (error) {
            console.error(error);
            return res.status(500).send('Internal Server Error');
          }
        });

        app.get('/filter_camera', async (req, res) => {
            try {
               const { filterdate } = req.query;

               const collection = client.db('AGV').collection('agv_camera');
               
               const startOfDay = new Date(filterdate);
               startOfDay.setUTCHours(0, 0, 0, 0);

               const endOfDay = new Date(filterdate);
               endOfDay.setUTCHours(23, 59, 59, 999);
              
               const query = [
                { $match: { timestamp: { $gte: startOfDay, $lte: endOfDay } } },
                { $project: { _id: 0, data: 1, timestamp: 1 } },
                { $sort: { timestamp: -1 } }
                ];    
                const data_from_collection = await collection.aggregate(query).toArray();
                const imageData = data_from_collection.map(d => ({ data: d.data, timestamp: new Date(d.timestamp) }));
         
               return res.json({ images_data: imageData });
             } catch (error) {
               console.error(error);
               return res.status(500).send('Internal Server Error');
             }
           });
         
           app.get('/filter_line_tracker', async (req, res) => {
             try {
                const { filterdate } = req.query;

                const collection = client.db('AGV').collection('agv_line_tracker');
                const startOfDay = new Date(filterdate);
                startOfDay.setUTCHours(0, 0, 0, 0);
            
                const endOfDay = new Date(filterdate);
                endOfDay.setUTCHours(23, 59, 59, 999);
              
                const query = [
                  { $match: { timestamp: { $gte: startOfDay, $lte: endOfDay } } },
                  { $project: { _id: 0, line_status: 1, timestamp: 1 } },
                  { $sort: { timestamp: -1 } }
                ];   
                
                const data = await collection.aggregate(query).toArray();
                const lineTrackerData = data.map(d => ({ line_status: d.line_status, timestamp: new Date(d.timestamp) }));
          
                // Return the sensor data in the response
                return res.json({ line_status_data: lineTrackerData });
              } catch (error) {
                console.error(error);
                return res.status(500).send('Internal Server Error');
              }
            });
         
            app.get('/filter_obstacles_detected', async (req, res) => {
             try {
                const { filterdate } = req.query;

                const collection = client.db('AGV').collection('agv_obstacles_detected');
                const startOfDay = new Date(filterdate);
                startOfDay.setUTCHours(0, 0, 0, 0);
            
                const endOfDay = new Date(filterdate);
                endOfDay.setUTCHours(23, 59, 59, 999);
              
            
                const query = [
                  { $match: { timestamp: { $gte: startOfDay, $lte: endOfDay } } },
                  { $project: { _id: 0, data: 1, timestamp: 1 } },
                  { $sort: { timestamp: -1 } }
                ];    
                const data_from_collection = await collection.aggregate(query).toArray();
                const imageData = data_from_collection.map(d => ({ data: d.data, timestamp: new Date(d.timestamp) }));

                return res.json({ images_data: imageData });
              } catch (error) {
                console.error(error);
                return res.status(500).send('Internal Server Error');
              }
            });
          
            app.get('/filter_ultrasonic', async (req, res) => {
              try {
                const { filterdate } = req.query;

                 const collection = client.db('AGV').collection('agv_ultrasonic');
                 const startOfDay = new Date(filterdate);
                 startOfDay.setUTCHours(0, 0, 0, 0);

                const endOfDay = new Date(filterdate);
                endOfDay.setUTCHours(23, 59, 59, 999);
  
                const query = [
                    { $match: { timestamp: { $gte: startOfDay, $lte: endOfDay } } },
                    { $project: { _id: 0, left_distance: 1, right_distance: 1, back_distance: 1,front_distance: 1, timestamp: 1 } },
                    { $sort: { timestamp: -1 } }
                    ];   
                    
                    const data = await collection.aggregate(query).toArray();
                    const leftDistance = data.map(d => ({ left_distance: d.left_distance, timestamp: new Date(d.timestamp) }));
                    const rightDistance = data.map(d => ({ right_distance: d.right_distance, timestamp: new Date(d.timestamp) }));
                    const backDistance = data.map(d => ({ back_distance: d.back_distance, timestamp: new Date(d.timestamp) }));
                    const frontDistance = data.map(d => ({ front_distance: d.front_distance, timestamp: new Date(d.timestamp) }));
           
                    // Return the sensor data in the response
                    return res.json({ left_distance_data: leftDistance,
                        right_distance_data: rightDistance,
                        back_distance_data: backDistance,
                        front_distance_data: frontDistance
                    });
                    } catch (error) {
                        console.error(error);
                        return res.status(500).send('Internal Server Error');
                    }
             });
             app.get('/filter_resources', async (req, res) => {
                 try {
                    const { filterdate } = req.query;
                    const collection = client.db('AGV').collection('agv_resources');
                    const startOfDay = new Date(filterdate);
                    startOfDay.setUTCHours(0, 0, 0, 0);

                    const endOfDay = new Date(filterdate);
                    endOfDay.setUTCHours(23, 59, 59, 999);
                    
                    const query = [
                    { $match: { timestamp: { $gte: startOfDay, $lte: endOfDay } } },
                    { $project: { _id: 0, memoryConsumed: 1,memoryAvailable: 1, cpuUtilization: 1,timestamp: 1 } },
                    { $sort: { timestamp: -1 } }
                    ];   
                
                    const data = await collection.aggregate(query).toArray();
                    let memoryConsumed = 0;
                    let  memoryAvailable = 400;
                    let cpuUtilization=0;
                    if(data.length>0){
                        // Calculate the average of each property of the objects in the `data` array
                        memoryConsumed = data.reduce((acc, curr) => acc + curr.memoryConsumed, 0) / data.length;
                        memoryAvailable = data.reduce((acc, curr) => acc + curr.memoryAvailable, 0) / data.length;
                        cpuUtilization = data.reduce((acc, curr) => acc + curr.cpuUtilization, 0) / data.length;
                    }     
                     // Return the memory and CPU utilization values in the response
                    return res.json({ memory: {
                        consumed: memoryConsumed,
                        available: memoryAvailable
                        },
                        cpu: {
                            utilization: cpuUtilization
                        }
                    });
                    } catch (error) {
                    console.error(error);
                    return res.status(500).send('Internal Server Error');
                  }
                });
              
                app.get('/filter_wheels', async (req, res) => {
                  try {
                     const { filterdate } = req.query;

                     const collection = client.db('AGV').collection('agv_wheels');
                     const startOfDay = new Date(filterdate);
                     startOfDay.setUTCHours(0, 0, 0, 0);
                 
                     const endOfDay = new Date(filterdate);
                     endOfDay.setUTCHours(23, 59, 59, 999);
                   
                     const query = [
                       { $match: { timestamp: { $gte: startOfDay, $lte: endOfDay } } },
                       { $project: { _id: 0, direction: 1, speed: 1, timestamp: 1 } },
                       { $sort: { timestamp: -1 } }
                     ];
                     const data = await collection.aggregate(query).toArray();
                     const speedData = data.map(d => ({ speed: d.speed, timestamp: new Date(d.timestamp) }));
                     const directionData = data.map(d => ({ direction: d.direction, timestamp: new Date(d.timestamp) }));
                 
               
                     // Return the sensor data in the response
                     return res.json({speed_data: speedData,
                         direction_data: directionData });
                   } catch (error) {
                     console.error(error);
                     return res.status(500).send('Internal Server Error');
                   }
                 });      

app.listen(port);
