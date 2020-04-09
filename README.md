# Xello.RegistryConfigurationManager


Xello.RegistryConfigurationManager is a configuration provider that uses the windows registry for values.

For those not in the loop on why, let me try and explain.

When we are configuring our application generally we use octopus to change environment specific variables. This works fine when we only have to worry about a single application. But in reality, when we do a production deploy, we need to configure multiple applications at once.

For example, when doing a switch, if we were to use octopus paradigm of a rolling deploy, we would need to go to cc3.api and change all its configuration to point from stage to prod (eg. db connection strings, authstage to auth etc). We then have to do the same for every other application (cams.api, spark api, login etc). This becomes untenable for a number of reasons. One, it is hard to track where an application directory is. And two, it is not really desirable to totally reconfigure an application before moving it into prod.

So to address this we limit the amount of config we do when moving into production, and we bypass the need to know where the application folders are by currently updating hostfiles. By updating the hostfiles, we can essentially do a system wide reconfig of all applications on the host, and limit any changed configuration to a single location (instead of across multiple apps)

This worked fine and dandy until we moved to azure. Moving to azure broke this in a couple of ways. First the application gateways ip is not reachable from the vm hosts. So we could not route domains to ips via the hostfile. Second we no longer had control over the sql servers ip and needed to use an FQDN which we could not map from a hostfile

So we still needed a way to do system wide reconfig, but without the use of a hostfile. This is where the this repo comes in. By putting config in the registry (like just about every other windows app in existence), we can allow all apps to have a single point of configuration.

We have discussed other ways of doing this, here are some of them :

Creating a file somewhere on the file system
- the three major pain point were app permissioning, consistency of location and the safety of the configuration

Azure configuration service
- this was a major consideration, and still is. Honestly, my biggest issue here is that if the service is down, our whole app is down. We know we are moving to azure, but azure is not a monolith, and if one service goes down, it doesnt mean they all do. There is also some concern about the latency loading a something as critical as config

The Registry
- the biggest problem with the registry is that the values will be hard to find. Not always a bad thing, because we really shouldnt be manually changing config.

In the end we might take a hybrid approach, which is why we implemented this as a ConfiguraitonProvider. AspNetCore allows us to stack providers, so we can basically say, hey if azure is down, fall back on registry.

