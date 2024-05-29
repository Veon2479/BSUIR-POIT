----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/05/2023 09:01:35 PM
-- Design Name: 
-- Module Name: DPLatch - Behavioral
-- Project Name: 
-- Target Devices: 
-- Tool Versions: 
-- Description: 
-- 
-- Dependencies: 
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
-- 
----------------------------------------------------------------------------------


library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--use IEEE.NUMERIC_STD.ALL;

-- Uncomment the following library declaration if instantiating
-- any Xilinx leaf cells in this code.
--library UNISIM;
--use UNISIM.VComponents.all;

entity DPLatch is
    Port (
        ps, d, e: in std_logic;
        q : out std_logic
    );
end DPLatch;

architecture Behavioral of DPLatch is
    signal v : std_logic;
begin

    main: process(ps, d, e)
    begin
        if ps = '1' then
            v <= '1';
        elsif e = '1' then 
            v <= d; 
        end if;    
    end process;
    
    q <= v;
end Behavioral;
