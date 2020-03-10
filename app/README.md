

https://michaelscodingspot.com/c-job-queues/

https://michaelscodingspot.com/performance-of-producer-consumer/


In Part 1 and Part 2 we went over what are Job Queues, why they are so important and how to implement them with several methods. Some of those methods were thread-pool implementations, BlockingCollection implementations, Reactive Extensions, and System.Threading.Channels.

Job Queues are also referred to as the Producer-consumer problem. We’ll be adding jobs to the queue (producing) and handling them (consuming) in a First-In-First-Out (FIFO) order. With some variations.

Let’s talk about those variations for a moment. Software development is versatile (thank god), otherwise there wouldn’t be so many of us. Each project is different and requires customization. Some common Job Queue variations might be:

Prioritizing jobs
Having different handlers for different types of job (publisher-subscriber)
Handling jobs in multiple threads
Limiting Job Queue capacity
Having the queue stored in an external queue like Kafka or RabbitMQ.