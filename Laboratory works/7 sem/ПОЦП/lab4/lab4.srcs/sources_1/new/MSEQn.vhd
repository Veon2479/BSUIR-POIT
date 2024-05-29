----------------------------------------------------------------------------------
-- Company: 
-- Engineer: 
-- 
-- Create Date: 11/06/2023 04:02:54 PM
-- Design Name: 
-- Module Name: MSEQn - Behavioral
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

entity MSEQn is
    Generic (pol: std_logic_vector := "1100001");
    Port (
        CLK: in std_logic;
        RST: in std_logic;
        Pout: out std_logic_vector(0 to pol'high-1)
    );
end MSEQn;

architecture Behavioral of MSEQn is
    signal sreg, sdat : std_logic_vector(0 to pol'high-1);
begin
    m: process(CLK, RST) 
    begin
        if RST='1' then
            sreg <= (others => '0');
        elsif rising_edge(CLK) then 
            sreg <= sdat;
        end if;
    end process;
    
    d: process(sreg)
        variable newbit, zerostate : std_logic;
    begin
        newbit := '0';
        zerostate := '0';
        
        for i in 0 to pol'high-2 loop 
            zerostate := zerostate or sreg(i);
            if pol(i+1) = '1' then 
                newbit := newbit xor sreg(i);
            end if;
        end loop;
        
        zerostate := not zerostate;
        newbit := zerostate xor newbit xor sreg(pol'high-1);
        
        sdat <= newbit & sreg(0 to pol'high-2);
    end process;
    
    Pout <= sreg;

end Behavioral;
